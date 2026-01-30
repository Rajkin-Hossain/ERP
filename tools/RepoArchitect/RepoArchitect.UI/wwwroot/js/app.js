let graphData = null;
let cy = null;

const scanStatus = document.getElementById("scanStatus");
const refreshButton = document.getElementById("refreshButton");
const searchBox = document.getElementById("searchBox");
const filterCycles = document.getElementById("filterCycles");
const filterTransitive = document.getElementById("filterTransitive");
const groupByFolder = document.getElementById("groupByFolder");
const details = document.getElementById("projectDetails");

const tabs = document.querySelectorAll(".tab");
const panels = document.querySelectorAll(".tab-panel");

tabs.forEach(tab => {
  tab.addEventListener("click", () => {
    tabs.forEach(t => t.classList.remove("active"));
    panels.forEach(panel => panel.classList.remove("active"));
    tab.classList.add("active");
    document.getElementById(`tab-${tab.dataset.tab}`).classList.add("active");
  });
});

refreshButton.addEventListener("click", async () => {
  await loadGraph(true);
});

[searchBox, filterCycles, filterTransitive, groupByFolder].forEach(el => {
  el.addEventListener("input", () => renderGraph());
  el.addEventListener("change", () => renderGraph());
});

async function loadGraph(force = false) {
  scanStatus.textContent = "Scanning…";
  const endpoint = force ? "/api/refresh" : "/api/graph";
  const response = await fetch(endpoint);
  graphData = await response.json();
  scanStatus.textContent = `Last scan: ${new Date(graphData.scannedAt).toLocaleTimeString()}`;
  renderGraph();
  renderPackages();
  renderViolations();
}

function renderGraph() {
  if (!graphData) return;

  const cycleNodes = new Set((graphData.cycles || []).flat());
  const nodes = graphData.graph.nodes
    .filter(node => !filterCycles.checked || cycleNodes.has(node.id))
    .map(node => ({ data: node }));

  const edges = graphData.graph.edges.map(edge => ({ data: edge }));
  const transitiveEdges = filterTransitive.checked ? buildTransitiveEdges(graphData.graph) : [];

  cy = cytoscape({
    container: document.getElementById("cytoscape"),
    elements: [...nodes, ...edges, ...transitiveEdges],
    style: [
      {
        selector: "node",
        style: {
          "background-color": "#4f46e5",
          "label": "data(label)",
          "color": "#0f172a",
          "text-valign": "center",
          "text-halign": "center",
          "font-size": 11,
          "text-outline-color": "#eef2ff",
          "text-outline-width": 2
        }
      },
      {
        selector: "edge",
        style: {
          "curve-style": "bezier",
          "line-color": "#94a3b8",
          "target-arrow-color": "#94a3b8",
          "target-arrow-shape": "triangle"
        }
      },
      {
        selector: ".transitive",
        style: {
          "line-style": "dashed",
          "line-color": "#cbd5f5",
          "target-arrow-color": "#cbd5f5"
        }
      },
      {
        selector: ".highlight",
        style: {
          "background-color": "#f97316",
          "text-outline-color": "#fde68a"
        }
      }
    ],
    layout: {
      name: groupByFolder.checked ? "breadthfirst" : "cose",
      padding: 20,
      animate: false
    }
  });

  cy.on("tap", "node", evt => {
    const node = evt.target.data();
    const project = graphData.projects.find(p => p.name === node.id);
    renderDetails(project);
  });

  applySearchHighlight();
}

function applySearchHighlight() {
  const query = searchBox.value.toLowerCase();
  if (!cy) return;
  cy.nodes().removeClass("highlight");
  if (!query) return;
  cy.nodes().forEach(node => {
    if (node.data("label").toLowerCase().includes(query)) {
      node.addClass("highlight");
    }
  });
}

function buildTransitiveEdges(graph) {
  const adjacency = new Map();
  graph.edges.forEach(edge => {
    if (!adjacency.has(edge.source)) {
      adjacency.set(edge.source, new Set());
    }
    adjacency.get(edge.source).add(edge.target);
  });

  const transitive = [];
  graph.nodes.forEach(node => {
    const visited = new Set();
    const stack = [...(adjacency.get(node.id) || [])];
    while (stack.length > 0) {
      const current = stack.pop();
      if (visited.has(current)) continue;
      visited.add(current);
      (adjacency.get(current) || []).forEach(next => stack.push(next));
      if (!adjacency.get(node.id)?.has(current)) {
        transitive.push({ data: { source: node.id, target: current }, classes: "transitive" });
      }
    }
  });
  return transitive;
}

function renderDetails(project) {
  if (!project) {
    details.textContent = "Select a node to see details.";
    return;
  }

  const packages = project.packageReferences.map(pkg => `${pkg.id} ${pkg.version || ""}`.trim());
  const references = project.projectReferences.map(ref => ref.projectName || ref.include);

  details.innerHTML = `
    <strong>${project.name}</strong><br />
    <span class="muted">${project.relativePath}</span><br /><br />
    <strong>Target Frameworks:</strong> ${project.targetFrameworks.join(", ") || "n/a"}<br /><br />
    <strong>Project References:</strong>
    <ul>${references.map(ref => `<li>${ref}</li>`).join("") || "<li>None</li>"}</ul>
    <strong>Package References:</strong>
    <ul>${packages.map(pkg => `<li>${pkg}</li>`).join("") || "<li>None</li>"}</ul>
  `;
}

function renderPackages() {
  if (!graphData) return;
  const perProject = document.getElementById("packagesPerProject");
  const topPackages = document.getElementById("topPackages");
  const versionConflicts = document.getElementById("versionConflicts");

  const packageCounts = {};
  const versionMap = {};

  perProject.innerHTML = "";
  graphData.projects.forEach(project => {
    const count = project.packageReferences.length;
    perProject.innerHTML += `<div class="table-row"><span>${project.name}</span><span>${count}</span></div>`;
    project.packageReferences.forEach(pkg => {
      packageCounts[pkg.id] = (packageCounts[pkg.id] || 0) + 1;
      versionMap[pkg.id] ??= new Set();
      if (pkg.version) {
        versionMap[pkg.id].add(pkg.version);
      }
    });
  });

  const sortedPackages = Object.entries(packageCounts).sort((a, b) => b[1] - a[1]);
  topPackages.innerHTML = sortedPackages
    .slice(0, 10)
    .map(([name, count]) => `<div class="table-row"><span>${name}</span><span>${count}</span></div>`)
    .join("");

  const conflicts = Object.entries(versionMap)
    .filter(([, versions]) => versions.size > 1)
    .map(([name, versions]) => ({ name, versions: [...versions] }));

  versionConflicts.innerHTML = conflicts.length
    ? conflicts.map(conflict => `
      <div class="table-row"><span>${conflict.name}</span><span>${conflict.versions.join(", ")}</span></div>
    `).join("")
    : "No version conflicts detected.";
}

function renderViolations() {
  const list = document.getElementById("violationsList");
  if (!graphData) return;

  if (!graphData.violations.length) {
    list.textContent = "No violations detected.";
    return;
  }

  list.innerHTML = graphData.violations.map(v => `
    <div class="table-row">
      <span><strong>${v.ruleName}</strong>: ${v.fromProject} -> ${v.toProject}</span>
      <span>${v.reason}</span>
    </div>
  `).join("");
}

const exportJson = document.getElementById("exportJson");
const exportDot = document.getElementById("exportDot");

exportJson.addEventListener("click", () => downloadFile("/api/export/json", "repoarchitect.json"));
exportDot.addEventListener("click", () => downloadFile("/api/export/dot", "repoarchitect.dot"));

async function downloadFile(url, filename) {
  const response = await fetch(url);
  const blob = await response.blob();
  const link = document.createElement("a");
  link.href = URL.createObjectURL(blob);
  link.download = filename;
  link.click();
  URL.revokeObjectURL(link.href);
}

const aiForm = document.getElementById("aiForm");
const aiOutput = document.getElementById("aiOutput");

aiForm.addEventListener("submit", async event => {
  event.preventDefault();
  aiOutput.textContent = "Generating insights...";
  const payload = {
    apiKey: document.getElementById("aiKey").value,
    model: document.getElementById("aiModel").value
  };

  const response = await fetch("/api/ai/insights", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload)
  });

  if (!response.ok) {
    aiOutput.textContent = await response.text();
    return;
  }

  const result = await response.json();
  aiOutput.textContent = result.message;
});

window.addEventListener("load", loadGraph);

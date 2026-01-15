namespace ERP.SharedKernal.Exceptions;

public sealed class MongoDbException(string message) : Exception(message) { }
namespace test.Common.Exceptions;

public class NotFoundException<T>(string entityName, T entityId) : Exception($"{entityName} with {entityId} not found");

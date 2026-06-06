namespace Test.Backend.Common.Exceptions;

public class NotFoundException(string entityName, string entityId)
    : Exception($"{entityName} with {entityId} not found");
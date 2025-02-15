namespace OFinance.Application.DTOs;


public record ItemDto(
         Guid Id,
         Guid Account,
         string Title,
         double Amount,
         string Category,
         string Type,
         string Description,
         DateTime CreatedAt,
         DateTime? UpdatedAt
);

public record CreateItemDto(
   Guid Account,
   string Title,
   double Amount,
   string Category,
   string Type,
   string Description
);

public record UpdateItemDto( 
   double Amount,
   string Category,
   string Type,
   string Description
);
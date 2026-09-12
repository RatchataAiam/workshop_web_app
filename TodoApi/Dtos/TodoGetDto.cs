namespace TodoApi.Dtos;

public record TodoGetDto(
    int ID,
    string Title,
    bool IsCompleted
);
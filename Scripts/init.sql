USE omniflowhub;

CREATE TABLE IF NOT EXISTS Documents (
    Id CHAR(36) NOT NULL PRIMARY KEY,
    FileName VARCHAR(255) NOT NULL,
    ContentType VARCHAR(100) NOT NULL,
    S3Key VARCHAR(500) NOT NULL,
    Status INT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    ProcessedAt DATETIME NULL,
    ErrorMessage TEXT NULL
);
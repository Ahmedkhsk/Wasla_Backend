using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    public partial class Message : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add isEdited column if it doesn't exist
            migrationBuilder.Sql(@"
                IF COL_LENGTH('Messages', 'isEdited') IS NULL
                BEGIN
                    ALTER TABLE Messages ADD isEdited BIT NOT NULL DEFAULT 0;
                END
            ");

            // Add receiverId column if it doesn't exist
            migrationBuilder.Sql(@"
                IF COL_LENGTH('Messages', 'receiverId') IS NULL
                BEGIN
                    ALTER TABLE Messages ADD receiverId NVARCHAR(MAX) NOT NULL DEFAULT '';
                END
            ");

            // Alter senderId column in Chats if needed
            migrationBuilder.Sql(@"
                IF COL_LENGTH('Chats', 'senderId') IS NOT NULL
                BEGIN
                    ALTER TABLE Chats ALTER COLUMN senderId NVARCHAR(450) NOT NULL;
                END
            ");

            // Alter receiverId column in Chats if needed
            migrationBuilder.Sql(@"
                IF COL_LENGTH('Chats', 'receiverId') IS NOT NULL
                BEGIN
                    ALTER TABLE Chats ALTER COLUMN receiverId NVARCHAR(450) NOT NULL;
                END
            ");

            // Create index on receiverId if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Chats_receiverId' AND object_id = OBJECT_ID('Chats'))
                BEGIN
                    CREATE INDEX IX_Chats_receiverId ON Chats(receiverId);
                END
            ");

            // Create index on senderId if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Chats_senderId' AND object_id = OBJECT_ID('Chats'))
                BEGIN
                    CREATE INDEX IX_Chats_senderId ON Chats(senderId);
                END
            ");

            // Add foreign key FK_Chats_AspNetUsers_receiverId if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Chats_AspNetUsers_receiverId')
                BEGIN
                    ALTER TABLE Chats
                    ADD CONSTRAINT FK_Chats_AspNetUsers_receiverId
                    FOREIGN KEY (receiverId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE;
                END
            ");

            // Add foreign key FK_Chats_AspNetUsers_senderId if it doesn't exist
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Chats_AspNetUsers_senderId')
                BEGIN
                    ALTER TABLE Chats
                    ADD CONSTRAINT FK_Chats_AspNetUsers_senderId
                    FOREIGN KEY (senderId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE;
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key FK_Chats_AspNetUsers_receiverId if exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Chats_AspNetUsers_receiverId')
                BEGIN
                    ALTER TABLE Chats DROP CONSTRAINT FK_Chats_AspNetUsers_receiverId;
                END
            ");

            // Drop foreign key FK_Chats_AspNetUsers_senderId if exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Chats_AspNetUsers_senderId')
                BEGIN
                    ALTER TABLE Chats DROP CONSTRAINT FK_Chats_AspNetUsers_senderId;
                END
            ");

            // Drop index IX_Chats_receiverId if exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Chats_receiverId' AND object_id = OBJECT_ID('Chats'))
                BEGIN
                    DROP INDEX IX_Chats_receiverId ON Chats;
                END
            ");

            // Drop index IX_Chats_senderId if exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Chats_senderId' AND object_id = OBJECT_ID('Chats'))
                BEGIN
                    DROP INDEX IX_Chats_senderId ON Chats;
                END
            ");

            // Drop column isEdited if exists
            migrationBuilder.Sql(@"
                IF COL_LENGTH('Messages', 'isEdited') IS NOT NULL
                BEGIN
                    ALTER TABLE Messages DROP COLUMN isEdited;
                END
            ");

            // Drop column receiverId if exists
            migrationBuilder.Sql(@"
                IF COL_LENGTH('Messages', 'receiverId') IS NOT NULL
                BEGIN
                    ALTER TABLE Messages DROP COLUMN receiverId;
                END
            ");

            // Revert Chats columns to nvarchar(max) if needed
            migrationBuilder.Sql(@"
                IF COL_LENGTH('Chats', 'senderId') IS NOT NULL
                BEGIN
                    ALTER TABLE Chats ALTER COLUMN senderId NVARCHAR(MAX) NOT NULL;
                END
            ");

            migrationBuilder.Sql(@"
                IF COL_LENGTH('Chats', 'receiverId') IS NOT NULL
                BEGIN
                    ALTER TABLE Chats ALTER COLUMN receiverId NVARCHAR(MAX) NOT NULL;
                END
            ");
        }
    }
}
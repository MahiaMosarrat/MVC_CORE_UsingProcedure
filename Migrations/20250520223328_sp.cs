using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientManagementCore.Migrations
{
    /// <inheritdoc />
    public partial class sp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TYPE ParamDoctorType AS TABLE (
            DoctorName NVARCHAR(50),
            TimeSlot  NVARCHAR(50)
            );
            GO
            CREATE OR ALTER PROCEDURE dbo.spInsertAppointment



            @PatientName NVARCHAR(50),
            @Age INT,
            @Gender NVARCHAR(10),
            @MobileNo NVARCHAR(15),
            @FirstVisit BIT,
            @AppointmentDate DATETIME,       
            @ConsultationFee DECIMAL(18,4),
            @SpecialistId INT,
            @ImageUrl NVARCHAR(100),
            @Doctors ParamDoctorType READONLY
            AS
            BEGIN
            BEGIN TRY
            DECLARE @LocalModules TABLE(
              DoctorName NVARCHAR(50),
            TimeSlot  NVARCHAR(50),
            AppointmentId INT
            )
            DECLARE @AppointmentId INT
            INSERT INTO dbo.Appointments(PatientName,Age,Gender,MobileNo,FirstVisit,AppointmentDate,ConsultationFee,SpecialistId,ImageUrl)VALUES(@PatientName,@Age,@Gender,@MobileNo,@FirstVisit,@AppointmentDate,@ConsultationFee,@SpecialistId,@ImageUrl);
            SET @AppointmentId=SCOPE_IDENTITY();

            INSERT INTO @LocalDoctors(DoctorName,TimeSlot,AppointmentId)
            SELECT DoctorName,TimeSlot,AppointmentId
            @Doctors

            INSERT INTO dbo.Doctors(DoctorName,TimeSlot,AppointmentId)
            SELECT DoctorName,TimeSlot,@AppointmentId
            FROM @LocalDoctors

            END TRY
            BEGIN CATCH
              Throw;
            END CATCH
            END
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.spInsertAppointment");
            migrationBuilder.Sql("DROP TYPE IF EXISTS ParamDoctorType");
        }
    }
}

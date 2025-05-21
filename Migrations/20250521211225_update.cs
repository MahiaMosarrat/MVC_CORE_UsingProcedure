using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatientManagementCore.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TYPE EditParamDoctorType AS TABLE (
            DoctorName NVARCHAR(50),
            TimeSlot  NVARCHAR(50)
            );
            GO
            CREATE OR ALTER PROCEDURE dbo.spUpdateAppointment

            @PatientName NVARCHAR(50),
            @Age INT,
            @Gender NVARCHAR(10),
            @MobileNo NVARCHAR(15),
            @FirstVisit BIT,
            @AppointmentDate DATETIME,       
            @ConsultationFee DECIMAL(18,4),
            @SpecialistId INT,
            @ImageUrl NVARCHAR(100),
            @Doctors EditParamDoctorType READONLY,
            @AppointmentId INT
            AS
            BEGIN
            BEGIN TRY
            
       
            UPDATE dbo.Appointments SET
            PatientName=@PatientName,
            Age=@Age,
            Gender=@Gender,
            MobileNo=@MobileNo,
            FirstVisit=@FirstVisit,
            AppointmentDate=@AppointmentDate,
            ConsultationFee=@ConsultationFee,
            SpecialistId=@SpecialistId,
            ImageUrl=@ImageUrl
            WHERE 
            AppointmentId= @AppointmentId;

            DELETE FROM dbo.Doctors
            WHERE AppointmentId= @AppointmentId;

            INSERT INTO dbo.Doctors(DoctorName,TimeSlot,AppointmentId)
            SELECT DoctorName,TimeSlot,@AppointmentId
            FROM @Doctors

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
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.spUpdateAppointment");
            migrationBuilder.Sql("DROP TYPE IF EXISTS EditParamDoctorType");
        }
    }
}

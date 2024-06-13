$(document).ready(function () {
    $('#StaffForm').on('submit', function (e) {
        e.stopImmediatePropagation();
        e.preventDefault();
        if ($(this).valid()) {
            $(this).ajaxSubmit({
                success: function (data) {
                    AddStaffSucess(data);
                }
            });
        }

    });
    $('#DoctorForm').on('submit', function (e) {
        e.stopImmediatePropagation();
        e.preventDefault();
        if ($(this).valid()) {
            $(this).ajaxSubmit({
                success: function (data) {
                    AddDoctorSucess(data);
                }
            });
        }

    });
    $('#PatientForm').on('submit', function (e) {
        e.stopImmediatePropagation();
        e.preventDefault();
        if ($(this).valid()) {
            $(this).ajaxSubmit({
                success: function (data) {
                    AddPatientSucess(data);
                }
            });
        }

    });

});
function DeleteStaffById(staffId) {
    $.confirm({
        title: 'Alert!',
        content: 'Do you want to delete this record?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Yes',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax('/AdminDashboard/DeleteStaffById', {
                        type: 'POST',  // http method
                        data: { StaffId: staffId },  // data to submit
                        success: function (data) {
                            if (data === "success") {
                                alertG("Deleted Successfully!");
                                $("#row_" + staffId).remove();
                                setTimeout(function () {
                                    location.reload();
                                }, 2000);
                            }
                        },
                        error: function () {
                            alertR('Error');
                        }
                    });
                }
            },
            close: function () {

            }
        }
    });

}

function DeleteDocById(docId) {
    $.confirm({
        title: 'Alert!',
        content: 'Do you want to delete this record?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Yes',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax('/AdminDashboard/DeleteDoctorById', {
                        type: 'POST',  // http method
                        data: { DocId: docId },  // data to submit
                        success: function (data) {
                            if (data === "success") {
                                alertG("Deleted Successfully!");
                                $("#row_" + docId).remove();
                                setTimeout(function () {
                                    location.reload();
                                }, 2000);
                            }
                        },
                        error: function () {
                            alertR('Error');
                        }
                    });
                }
            },
            close: function () {

            }
        }
    });

}
function DeletePatientById(id) {
    debugger;
    $.confirm({
        title: 'Alert!',
        content: 'Do you want to delete this record?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Yes',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax('/AdminDashboard/DeletePatientById', {
                        
                        type: 'POST',  // http method
                        data: { PId: id },  // data to submit
                        success: function (data) {
                            if (data === "success") {
                                alertG("Deleted Successfully!");
                                $("#row_" + id).remove();
                                setTimeout(function () {
                                    location.reload();
                                }, 2000);
                            }
                        },
                        error: function () {
                            alertR('Error');
                        }
                    });
                }
            },
            close: function () {

            }
        }
    });

}
function DeleteRoomById(id) {
    $.confirm({
        title: 'Alert!',
        content: 'Do you want to delete this record?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Yes',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax('/AdminDashboard/DeleteRoomById', {
                        type: 'POST',  // http method
                        data: { roomId: id },  // data to submit
                        success: function (data) {
                            if (data === "success") {
                                alertG("Deleted Successfully!");
                                $("#row_" + id).remove();
                                setTimeout(function () {
                                    location.reload();
                                }, 2000);
                            }
                        },
                        error: function () {
                            alertR('Error');
                        }
                    });
                }
            },
            close: function () {

            }
        }
    });

}
function DeletePrescriptionById(id) {
    $.confirm({
        title: 'Alert!',
        content: 'Do you want to delete this record?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Yes',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax('/AdminDashboard/DeletePrescriptionById', {
                        type: 'POST',  // http method
                        data: { PreId: id },  // data to submit
                        success: function (data) {
                            if (data === "success") {
                                alertG("Deleted Successfully!");
                                $("#row_" + id).remove();
                                setTimeout(function () {
                                    location.reload();
                                }, 2000);
                            }
                        },
                        error: function () {
                            alertR('Error');
                        }
                    });
                }
            },
            close: function () {

            }
        }
    });

}
function DeleteTestById(id) {
    $.confirm({
        title: 'Alert!',
        content: 'Do you want to delete this record?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Yes',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax('/AdminDashboard/DeletePatientTestById', {
                        type: 'POST',  // http method
                        data: { testId: id },  // data to submit
                        success: function (data) {
                            if (data === "success") {
                                alertG("Deleted Successfully!");
                                $("#row_" + id).remove();
                                setTimeout(function () {
                                    location.reload();
                                }, 2000);
                            }
                        },
                        error: function () {
                            alertR('Error');
                        }
                    });
                }
            },
            close: function () {

            }
        }
    });

}

function DeleteAppointmentById(id) {
    $.confirm({
        title: 'Alert!',
        content: 'Do you want to delete this record?',
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Yes',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax('/AdminDashboard/DeleteAppointmentById', {
                        type: 'POST',  // http method
                        data: { AppointId: id },  // data to submit
                        success: function (data) {
                            if (data === "success") {
                                alertG("Deleted Successfully!");
                                $("#row_" + id).remove();
                                setTimeout(function () {
                                    location.reload();
                                }, 2000);
                            }
                        },
                        error: function () {
                            alertR('Error');
                        }
                    });
                }
            },
            close: function () {

            }
        }
    });

}
function EditStaffById(id) {
    $.ajax({
        url: "/AdminDashboard/EditStaffById",
        type: "POST",
        data: { StaffId: id },
        success: function (data) {
            $("#Staff_StaffId").val(data.StaffId);
            $("#Staff_Title option:selected").val(data.Title);
            $("#Staff_FirstName").val(data.FirstName);
            $("#Staff_LastName").val(data.LastName);
            if (data.DOB !== null) {
                data.DOB = moment(data.DOB).format("DD/MM/YYYY");
                $('#Staff_DOB').val(data.DOB);
            }
            //$("#Staff_DOB").val(data.DOB);
            $("#Staff_Phone").val(data.Phone);
            $("#Staff_Email").val(data.Email);
            $("#Staff_Gender option:selected").val(data.Gender);
            $("#Staff_Designation").val(data.Designation);
            //$("#Staff_JoiningDate").val(data.JoiningDate);
            if (data.JoiningDate !== null) {
                data.JoiningDate = moment(data.JoiningDate).format("DD/MM/YYYY");
                $('#Staff_JoiningDate').val(data.JoiningDate);
            }
            $("#Staff_City option:selected").val(data.City);
            $("#Staff_Address").val(data.Address);
            if (data.Image !== null || data.Image !== "") {
                $("#Staff_Image").attr("src", data.Image);
            }
            else {
                $("#Staff_Image").attr("src", '/WebAssets/img/avatar-image_20190607082152.png');
            }
            $("#staffModel").modal('show');
        }
    });
}
function EditDoctorById(id) {
    $.ajax({
        url: "/AdminDashboard/EditDoctorById",
        type: "POST",
        data: { DoctorId: id },
        success: function (data) {
            $("#Doctor_DoctorID").val(data.DoctorID);
            $("#Doctor_FirstName").val(data.FirstName);
            $("#Doctor_LastName").val(data.LastName);
            if (data.DOB !== null) {
                data.DOB = moment(data.DOB).format("DD/MM/YYYY");
                $('#Doctor_DOB').val(data.DOB);
            }
            $("#Doctor_Phone").val(data.Phone);
            $("#Doctor_Email").val(data.Email);
            $("#Doctor_Gender").val(data.Gender);
            if (data.JoiningDate !== null) {
                data.JoiningDate = moment(data.JoiningDate).format("DD/MM/YYYY");
                $('#Doctor_JoiningDate').val(data.JoiningDate);
            }
            $("#Doctor_City").val(data.City);
            $("#Doctor_Address").val(data.Address);
            $("#User_UserName").val(data.UserName);
            $("#User_Password").val(data.Password);
            $("#ConfirmPassword").val(data.Password);
            $("#doctorModel").modal('show');
        }
    });
}
function EditPatientById(id) {
    $.ajax({
        url: "/AdminDashboard/EditPatientById",
        type: "POST",
        data: { PatientId: id },
        success: function (data) {
            $("#Patient_PatientID").val(data.PatientID);
            $("#Patient_FirstName").val(data.FirstName);
            $("#Patient_LastName").val(data.LastName);
            if (data.DOB !== null) {
                data.DOB = moment(data.DOB).format("DD/MM/YYYY");
                $('#Patient_DOB').val(data.DOB);
            }
            $("#Patient_Phone").val(data.Phone);
            $("#Patient_Email").val(data.Email);
            $("#Patient_Gender").val(data.Gender);
            if (data.JoiningDate !== null) {
                data.JoiningDate = moment(data.JoiningDate).format("DD/MM/YYYY");
                $('#Patient_JoiningDate').val(data.JoiningDate);
            }
            $("#Patient_City").val(data.City);
            $("#Patient_Age").val(data.Age);
            $("#Patient_Address").val(data.Address);
            $("#Patient_MariedStatus").val(data.MariedStatus);
            $("#Patient_BloodPressureStatus").val(data.BloodPressureStatus);
            $("#Patient_BloodGroup").val(data.BloodGroup);
            $("#Patient_Condtion").val(data.Condtion);
            $("#Patient_ConsultingRoom").val(data.ConsultingRoom);
            $("#Patient_AssignedDoc").val(data.AssignedDoc);
            $("#Patient_Status").val(data.Status);
            $("#patientModel").modal('show');
        }
    });
}
function EditPrescriptionById(id) {
    $.ajax({
        url: "/AdminDashboard/EditPrescriptionById",
        type: "POST",
        data: { PreId: id },
        success: function (data) {
            $("#Prescription_PatientPrescriptionID").val(data.PatientPrescriptionID);
            $("#Prescription_PatientID").val(data.PatientID);
            $("#Prescription_MedicineID").val(data.MedicineID);
            $("#Prescription_Reason").val(data.Reason);
            $("#Prescription_Dose").val(data.Dose);
            $("#Prescription_Unit").val(data.Unit);
            $("#startDate").val(data.StartDate);
            $("#endDate").val(data.EndDate);
            $("#Prescription_Comments").val(data.Comments);
            $("#preModel").modal('show');
        }
    });
}
function EditTestById(id) {
    $.ajax({
        url: "/AdminDashboard/EditTestById",
        type: "POST",
        data: { TId: id },
        success: function (data) {
            debugger;
            $("#LabTest_LabTestID").val(data.LabTestID);
            $("#LabTest_PatientID").val(data.PatientID);
            $("#LabTest_TestName").val(data.TestName);
            $("#LabTest_TestDate").val(data.TestDate);
            $("#LabTest_Report").val(data.Report);
            $("#LabTest_Status").val(data.Status);
            $("#LabTest_Remarks").val(data.Remarks);

            $("#testModel").modal('show');
        }
    });
}
function EditAppointmentById(id) {
    $.ajax({
        url: "/AdminDashboard/EditAppointmentById",
        type: "POST",
        data: { AppointId: id },
        success: function (data) {
            debugger;
            $("#Appointment_AppointmentID").val(data.AppointmentID);
            $("#Appointment_PatientID").val(data.PatientID);
            $("#Appointment_DoctorID").val(data.DoctorID);
            if (data.AppointmentDate !== null) {
                data.AppointmentDate = moment(data.AppointmentDate).format("DD/MM/YYYY");
                $('#AppointmentDate').val(data.AppointmentDate);
            }
            if (data.FromTime !== null) {
                data.FromTime = moment(data.FromTime).format("hh:mm");
                $('#FromTime').val(data.FromTime);
            }
            if (data.ToTime !== null) {
                data.ToTime = moment(data.ToTime).format("hh:mm");
                $('#ToTime').val(data.ToTime);
            }
            $("#appointModel").modal('show');
        }
    });
}

function AddDoctorSucess() {
    var txtSave = $("#btnSave").text();
    if (txtSave === "Save") {
        alertG("Doctor added successfully");
        setTimeout(function () {
            location.reload();
        }, 1000);
    }
}
function AddPatientSucess() {
    var txtSave = $("#btnSave").text();
    if (txtSave === "Save") {
        alertG("Patient added successfully");
        setTimeout(function () {
            location.reload();
        }, 1000);
    }
}
function CreateRoomChat() {
    $.ajax('/Chat/GetChatUsers',
        {

            type: 'POST',  // http method
            success: function (data, status, xhr) {

                $("#chatModel").html(data);
                $("#chatModel").modal('show');
            },
            error: function (jqXhr, textStatus, errorMessage) {
                alertR('Error')
            }
        });
}

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    var frmApprove = $("#frmApprove");
    var frmPublish = $("#frmPublish");
    var frmAttendance = $("#frmAttendance");

    var isApprovePage = $("#isApprovePage").val();
   
    var checkItemLength = $('.checkboxlistitem').length;

    var btnSubmit = document.createElement("input");
    btnSubmit.type = "submit";

    if (isApprovePage == null) {
        btnSubmit.id = "btnPublish";
        btnSubmit.value = "Publish";
    }
    else {
        btnSubmit.id = "btnApprove";
        btnSubmit.value = "Approve";
    }

    if ($('#isAttendancePage').val() != null) {
        frmAttendance.append(btnSubmit);
    }
    
    
    $(".checkAll").change(function () {  //"select all" change 
        var status = this.checked; // "select all" checked status
        $('.checkboxlistitem').each(function () { //iterate all listed checkbox items
            this.checked = status; //change ".checkbox" checked status
        });
        if (this.checked) {
            for (var i = 0; i < checkItemLength; i++) {

                //append input for training list page
                var input = document.createElement("input");
               
                input.name = "trainingArr";
                input.value = $('.checkboxlistitem')[i].id;
                input.id = "training" + $('.checkboxlistitem')[i].id;
                input.hidden = true;

                var tempInput = document.getElementById(input.id);
                if (tempInput == null) {
                    if (isApprovePage == null) {
                        frmPublish.append(input);
                    }
                    else {
                        frmApprove.append(input);
                    }
                }


                //append input in attendance list
                var volunteerInput = document.createElement("input");

                volunteerInput.name = "volunteerAttended";
                volunteerInput.value = $('.checkboxlistitem')[i].id;
                volunteerInput.id = "volunteer" + $('.checkboxlistitem')[i].id;
                volunteerInput.hidden = true;

                tempInput = document.getElementById(volunteerInput.id);
                if (tempInput == null) {
                    frmAttendance.append(volunteerInput);
                }
                
            }
        }
    });

    $('.checkboxlistitem').change(function () { //".checkbox" change 
        //uncheck "select all", if one of the listed checkbox item is unchecked
        if (this.checked == false) { //if this item is unchecked
            $(".checkAll")[0].checked = false; //change "select all" checked status to false
            var tempRemoveID = "training" + this.id;
            document.getElementById(tempRemoveID).remove();
        }

        //check "select all" if all checkbox items are checked
        if ($('.checkboxlistitem:checked').length == $('.checkboxlistitem').length) {
            $(".checkAll")[0].checked = true; //change "select all" checked status to true
        }

        if (this.checked == true) {
            var input = document.createElement("input");
            input.name = "trainingArr";
            input.value = this.id;
            input.id = "training" + this.id;
            input.hidden = true;

            var tempInput = document.getElementById(input.id);
            if (tempInput == null) {
                if (isApprovePage == null) {
                    frmPublish.append(input);
                }
                else {
                    frmApprove.append(input);
                }
            }


            //append input in attendance list
            var volunteerInput = document.createElement("input");

            volunteerInput.name = "volunteerAttended";
            volunteerInput.value = this.id;
            volunteerInput.id = "volunteer" + this.id;
            volunteerInput.hidden = true;

            tempInput = document.getElementById(volunteerInput.id);
            if (tempInput == null) {
                frmAttendance.append(volunteerInput);
            }
        }
    });

  
    if (isApprovePage == null) {
            frmPublish.append(btnSubmit);
        }
        else {
            frmApprove.append(btnSubmit);
    }

    //volunter register a new account
    function validateForm() {

        if ($("#termsConditions").is(':checked')) {
            return true;
        }
        else {
            alert("Please accept our terms and conditions");
            return false;
        }
    }
})

$(function () {
    $('#chkFilterDate').change();
    $('#chkFilterDuration').change();
    $('#chkFilterLocation').change();
    $('#chkFilterProgram').change();
});

// Sort by date
$('#chkFilterDate').change(function () {
    var isChecked = $(this).is(':checked');
    if (isChecked == true)
        $("#divDates").show();
    else
        $("#divDates").hide();
}).change();
// Sort by duration
$('#chkFilterDuration').change(function () {
    var isChecked = $(this).is(':checked');
    if (isChecked == true) {
        $("#divDurations").show();
    }
    else
        $("#divDurations").hide();
});
// Sort by Location checkbox
$('#chkFilterLocation').change(function () {
    var isChecked = $(this).is(':checked');
    if (isChecked == true) {
        $("#divFilterLocations").show();
        var ele = document.getElementById("divLocations");
        var myUrl = MyAppUrlSettings.addLocationUrl;
        addLocation(ele, myUrl);
    }
    else {
        $("#divFilterLocations").hide();
        $("#divLocations").empty();

    }
});
// Sort by Program checkbox
$('#chkFilterProgram').change(function () {
    var isChecked = $(this).is(':checked');
    if (isChecked == true) {
        $("#divFilterPrograms").show();
        var ele = document.getElementById("divPrograms");
        var myUrl = MyAppUrlSettings.addProgramUrl;
        addLocation(ele, myUrl);
    }
    else {
        $("#divFilterPrograms").hide();
        $("#divPrograms").empty();
    }
});
// Sort by Department checkbox
$('#chkFilterDepartment').change(function () {
    var isChecked = $(this).is(':checked');
    if (isChecked == true) {
        $("#divFilterDepartments").show();
        var ele = document.getElementById("divDepartments");
        var myUrl = MyAppUrlSettings.addDepartmentUrl;
        addLocation(ele, myUrl);
    }
    else {
        $("#divFilterDepartments").hide();
        $("#divDepartments").empty();
    }
});
// Sort by Population checkbox
$('#chkFilterPopulation').change(function () {
    var isChecked = $(this).is(':checked');
    if (isChecked == true) {
        $("#divFilterPopulations").show();
        var ele = document.getElementById("divPopulations");
        var myUrl = MyAppUrlSettings.addPopulationUrl;
        addLocation(ele, myUrl);
    }
    else {
        $("#divFilterPopulations").hide();
        $("#divPopulations").empty();
    }
});
// Sort by Type checkbox
$('#chkFilterType').change(function () {
    var isChecked = $(this).is(':checked');
    if (isChecked == true) {
        $("#divFilterTypes").show();
        var ele = document.getElementById("divTypes");
        var myUrl = MyAppUrlSettings.addTypeUrl;
        addLocation(ele, myUrl);
    }
    else {
        $("#divFilterTypes").hide();
        $("#divTypes").empty();
    }
});
// Sort by Role checkbox
$('#chkFilterRole').change(function () {
    var isChecked = $(this).is(':checked');
    if (isChecked == true) {
        $("#divFilterRoles").show();
        var ele = document.getElementById("divRoles");
        var myUrl = MyAppUrlSettings.addRoleUrl;
        addLocation(ele, myUrl);
    }
    else {
        $("#divFilterRoles").hide();
        $("#divRoles").empty();
    }
});
// Remove items from list dynamically
$("div.editableList").on("click", "a.deleteLink", function () { $(this).parents("div.removeMe:first").remove(); });
// Add new item to list dynamically
function addLocation(ctrl, getUrl) {
    $.ajax({
    type: 'GET',
    url: getUrl,
    cache: false,
    success: function (result) { $(ctrl).append(result);}
});}

// Initialize the dropdown menu
$(document).ready(function(){
    $('.dropdown-toggle').dropdown();
    $('body').click(function () {
        $('.dropdown.open').removeClass('open');        
    });
});

/* Fundtion to show 'Other' textbox when selected from Location dropdown
$(function () {
    $("#communityAction_LocationID").on("change", function () {
        // You're referring to the object itself, so you can use $(this).
        if ($("#communityAction_LocationID option:selected").text() == "Other") $(".divSubLocation").show();
        else $(".divSubLocation").hide();
    });
});
// Function to show 'Other' textbox when selected from Department dropdown
$(function () {
    $(".ddlDept").on("change", function () {
        alert('hi');
        // You're referring to the object itself, so you can use $(this).
        //if ($("#communityAction_LocationID option:selected").text() == "Other") $(".divSubLocation").show();
        //else $(".divSubLocation").hide();
    });
});
$(function () {
    // Set the end date to the same as the start date when the start date is set
    //$("#lblSDate").datepicker().on("changeDate", function (ev) {
        //alert("hi");
        //$("#lblEDate").datepicker("setDate", new Date);
    //});
});*/


//jquery.validator.methods.date = function (value, element) { if (value) { try { $.datepicker.parsedate('yyyy-mm-dd', value); } catch (ex) { return false; } } return true; };


$(function () {
            $("input[type='date']")
                   .datepicker()
            $("input[type='date']").each(function () { this.type = "text"; });
});


//var isdateinputSupported = function () {
//    var elem = document.createElement('input');
//    elem.setAttribute('type', 'date');
//    elem.value = 'foo';
//    return (elem.type == 'date' && elem.value != 'foo');
//}

//if (!isDateInputSupported())  // or.. !Modernizr.inputtypes.date
//    $('input[type="date"]').datepicker();
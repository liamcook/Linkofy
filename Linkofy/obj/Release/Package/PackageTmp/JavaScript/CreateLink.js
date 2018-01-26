$("#Obdomain").on("change", function () {

    var urllink = $('#Obdomain').val();
    var url = "/Links/TemplateData?ObDo=" + urllink

    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        url: url, // Variabel
        showLoader: true,
        success: changedo // Function
    });

});
function changedo(data) {
    data = $.parseJSON(data);
    console.log(data.domain)
    $('#IdentifierID').val(data.domain);
}

$("#Obpage").on("change", function () {

    var urlpg = $('#Obpage').val();
    var url = "/Links/ObPa?urlpg=" + urlpg

    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        url: url, // Variabel
        showLoader: true,
        success: changeob // Function
    });

});
function changeob(data) {
    data = $.parseJSON(data);
    console.log(data.client)
    $('#ClientID').val(data.client);
}
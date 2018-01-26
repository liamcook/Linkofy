$("#templateName").on("change", function () {

    var tempID = $(this).find('option:selected').val();
    var url = "/Identifiers/TemplateData/" + tempID;
    console.log()

    $.ajax({
        type: "GET",
        dataType: "json",
        contentType: "application/json",
        url: url, // Variabel
        showLoader: true,
        success: test // Function
    });

});
function test(data) {
    data = $.parseJSON(data);
    $('#subject').val(data.subject);
    $('#body').val(data.body);
}

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {url: '/admin/product/getall'},
        "columns": [
            { data: 'title', "width": "25%" },
            { data: 'isbn', "width": "15%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'author', "width": "15%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a href="" class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "25%"
            },
    ]

    });
}

///////////////////////////////////////////////////////////////
//var data = [
//    [
//        "Tiger Nixon",
//        "System Architect",
//        "Edinburgh",
//        "5421",
//        "2011/04/25",
//        "$3,120"
//    ],
//    [
//        "Garrett Winters",
//        "Director",
//        "Edinburgh",
//        "8422",
//        "2011/07/25",
//        "$5,300"
//    ]
//]

//$('#example').DataTable({
//    data: data
//});

//$(document).ready(function () {
//    $('#tblData').DataTable();
//})
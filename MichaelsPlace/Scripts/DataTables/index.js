$(document)
    .ready(function() {
        $('#index-data-table')
            .DataTable({
                    order: [[1, 'asc']],
                "columnDefs": [
                    { "orderable": false, "targets": 0 }
                ]
            });

        $(".delete-item").click(function(e) {
            var confirmed = confirm("Are you sure you want to delete this item?");
            if (!confirmed) {
                e.preventDefault();
                return false;
            }
        })
    });
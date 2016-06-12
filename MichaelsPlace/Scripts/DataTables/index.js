(function ($, _, window) {

    var buttonTemplate = _.template($("#_IndexItemButtons").html());

    var indexViewModel = {
        editLoaded: function(html) {
            $("#edit-modal-container").html(html);
            $.validator.unobtrusive.parse($("#edit-modal-container"));
            $("#edit-modal-container form").validateBootstrap(true);
            $("#edit-modal").modal('show');
        },
        editCompleted: function () {
            $("#edit-modal-container").html("");
            $("#edit-modal").modal('hide');
            indexViewModel.dataTable.ajax.reload();
        }
    }

    window.indexViewModel = indexViewModel;



    $(document)
        .ready(function() {
            var $table = $('#index-data-table'),
                ajaxData = $table.data('ajax-data'),
                urls = {
                    ajaxUrl: $table.data('ajax-url'),
                    editUrl: $table.data('edit-url'),
                    detailsUrl: $table.data('details-url'),
                    deleteUrl: $table.data('delete-url')
                },
            columns = $table.find('th')
                    .map(function(i, el) {
                        if (i === 0) {
                            return {
                                orderable: false,
                                render: function (data, type, row, meta) {
                                    return buttonTemplate(_.defaults(row, urls));
                                }
                            };
                        }
                        return {
                            orderable: false,
                            data: $(el).data('property')
                        };
                    });

            indexViewModel.dataTable = $table.DataTable({
                serverSide: true,
                processing: true,
                ajax: {
                    url: urls.ajaxUrl,
                    data: ajaxData
                },
                order: [[1, 'asc']],
                columns: columns
            });


            $('#index-data-table')
                .on('click',
                    '.delete-item',
                    function(e) {
                        var confirmed = confirm("Are you sure you want to delete this item?");
                        if (!confirmed) {
                            e.preventDefault();
                            return false;
                        }
                        return true;
                    });
        });
})($, _, window);

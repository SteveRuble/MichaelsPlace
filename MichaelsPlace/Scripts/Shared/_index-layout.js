(function ($, _, window) {


    $(document)
        .ready(function() {
            var $table = $('#index-data-table'),
                ajaxData = $table.data('ajax-data'),
                urls = {
                    ajaxUrl: $table.data('ajax-url'),
                },


                columns = $table.find('th')
                    .map(function (i, el) {
                        var $el = $(el);
                        if ($el.attr('id') === 'item-buttons') {
                            return {
                                orderable: false,
                                render: function(data, type, row, meta) {
                                    return buttonTemplate(_.defaults(row, urls));
                                }
                            };
                        }
                        var col = {
                            orderable: $el.data('orderable') || false,
                            searchable: $el.data('searchable') || false,
                            data: $(el).data('property')
                        };

                        if (col.data === "createdUtc") {
                            col.render = function(data, type, row, meta) {
                                var date = moment(data);
                                return date.format('MMMM Do YYYY, h:mm:ss a');
                            }
                        }
                        return col;
                    });

            var buttonTemplate = _.template($("#_IndexItemButtons").html());

            var vm = {
                modalLoaded: function (html, status, jqXHR) {
                    var $container = $("#ajax-modal");
                    if (jqXHR.status === 202) {
                        $container.find(".modal").modal('hide');
                        vm.reload();
                    } else {
                        $container.html(html);
                        $.validator.unobtrusive.parse($container);
                        $container.find("form").validateBootstrap(true);
                        $container.find(".modal").modal('show')
                            .on('hidden.bs.modal', function() {
                                $container.html("");
                            });
                    }
                },
                modalCompleted: function() {
                },
                reload: function() {
                    vm.dataTable.ajax.reload();
                },
                deleteClicked: function(e) {
                    var confirmed = confirm("Are you sure you want to delete this item?");
                    if (!confirmed) {
                        e.preventDefault();
                        return false;
                    }
                    var url = $(this).attr('formaction');
                    $.post(url).then(vm.reload);

                    return true;
                }
            }

            vm.dataTable = $table.DataTable({
                serverSide: true,
                processing: true,
                responsive: true,
                ajax: {
                    url: urls.ajaxUrl,
                    data: ajaxData
                },
                order: [[1, 'asc']],
                columns: columns,
                initComplete: function() {
                    $('#create-button').appendTo('#index-data-table_length');
                }
            });


            $('#index-data-table')
                .on('click',
                    '.delete-item',
                    vm.deleteClicked);

            window.indexViewModel = vm;
        });


})($, _, window);

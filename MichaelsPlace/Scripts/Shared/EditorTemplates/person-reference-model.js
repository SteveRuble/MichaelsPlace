var REGEX_EMAIL = '([a-z0-9!#$%&\'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&\'*+/=?^_`{|}~-]+)*@' +
                  '(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)';

$('#select-to').selectize({
    persist: false,
    maxItems: 1,
    valueField: 'email',
    labelField: 'name',
    searchField: ['name', 'email'],
    options: [],
    render: {
        item: function (item, escape) {
            return '<div>' +
                (item.name ? '<span class="name">' + escape(item.name) + '</span>' : '') +
                (item.email ? '<span class="email">' + escape(item.email) + '</span>' : '') +
            '</div>';
        },
        option: function (item, escape) {
            var label = item.name || item.email;
            var caption = item.name ? item.email : null;
            return '<div>' +
                '<label>' + escape(label) + '</label>' +
                (caption ? '<em>' + escape(caption) + '</em>' : '') +
            '</div>';
        }
    },
    create: function (input) {
        if ((new RegExp('^' + REGEX_EMAIL + '$', 'i')).test(input)) {
            return { email: input };
        }
        var match = input.match(new RegExp('^([^<]*)\<' + REGEX_EMAIL + '\>$', 'i'));
        if (match) {
            return {
                email: match[2],
                name: $.trim(match[1])
            };
        }
        alert('Invalid email address.');
        return false;
    },
    load: function (query, callback) {
        if (!query.length) return callback();
        $.ajax({
            url: '/People/JsonReferenceSearch/?query=' + encodeURIComponent(query),
            type: 'GET',
            error: function () {
                callback();
            },
            success: function (res) {
                callback(res.map(function(p) {
                    return {
                        email: p.Email,
                        name: p.Name,
                        id: p.Id
                    };
                }));
            }
        });
    }
});
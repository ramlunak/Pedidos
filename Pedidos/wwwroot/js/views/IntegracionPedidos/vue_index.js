
var content = new Vue({
    el: '#content',
    created: function () {
        console.log($('#btnAdd'));
    },
    data: {
        message: 'Hello Vue!',
        barrios: [
            { text: 'Learn JavaScript' },
            { text: 'Learn Vue' },
            { text: 'Build something awesome' }
        ]
    }
})

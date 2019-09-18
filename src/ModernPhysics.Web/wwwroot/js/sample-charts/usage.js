var ctx = document.getElementById('chart1').getContext('2d');
var chart = new Chart(ctx, {
    type: 'doughnut',
    data: {
        labels: [
            'Poland',
            'UK',
            'Other'
        ],

        datasets: [{
            label: 'Visitors by country', 
            data: [40, 10, 8],
            backgroundColor: [
                'rgba(255, 99, 132, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(54, 162, 235, 1)'
            ]
        }]
    },
    options: {
        circumference: Math.PI,
        rotation: Math.PI
    }
});



//Chart2
var ctx = document.getElementById('chart2').getContext('2d');
var chart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: [
            "January","February","March","April","May","June","July"
        ],

        datasets: [{
            label: 'Visitors',
            data: [27, 20, 30, 20, 11, 24, 34],
            fill: false,
            borderColor: 'rgba(54, 162, 235, 1)',
            lineTension: 0.3
        }]
    },
    options: {}
});




//chart 3
var ctx = document.getElementById('chart3').getContext('2d');
var chart = new Chart(ctx, {
    type: 'horizontalBar',
    data: {
        labels: ['/', '/index', '/pages', '/usage', '/health', '/metrics'],
        datasets: [{
            label: 'Visitors by page',
            data: [40, 32, 17, 24, 8, 20],
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
                'rgba(153, 102, 255, 0.2)',
                'rgba(255, 159, 64, 0.2)'
            ],
            borderColor: [
                'rgba(255, 99, 132, 1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(255, 159, 64, 1)'
            ],
            borderWidth: 1
        }]
    },
    options: {}
});


//chart 4

var ctx = document.getElementById('chart4').getContext('2d');
var chart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: [
            "0:00","3:00","6:00","9:00","12:00","15:00","18:00","21:00"
        ],

        datasets: [{
            label: 'Visitors',
            backgroundColor: 'rgba(235, 220, 35, 0.5)',
            data: [7, 3, 2, 8, 14, 20, 34, 19],
            fill: true,
            borderColor: 'rgba(235, 220, 35, 1)',
            lineTension: 0.3
        }]
    },
    options: {}
});
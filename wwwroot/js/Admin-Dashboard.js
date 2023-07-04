var options = {
    chart: {
        height: 180,
        width: 130,
        type: "radialBar",
    },
    series: [ 97, 61],
    plotOptions: {
        radialBar: {
            dataLabels: {
                total: {
                    show: true,
                    label: 'TOTAL'
                }
            }
        }
    },
    labels: ['TEAM A', 'TEAM B', 'TEAM C', 'TEAM D']
};


var chart = new ApexCharts(document.querySelector("#income"), options);

chart.render();
var options2 = {
    chart: {
        height: 180,
        width: 130,
        type: "radialBar",
    },
    series: [97, 61],
    plotOptions: {
        radialBar: {
            dataLabels: {
                total: {
                    show: true,
                    label: 'TOTAL'
                }
            }
        }
    },
    labels: ['TEAM A', 'TEAM B', 'TEAM C', 'TEAM D']
}

var chart = new ApexCharts(document.querySelector("#Balance"), options2);

chart.render();

options3 = {
    chart: {
        width: 250,
        height:300,
        type: 'bar',
    },
    series: [{
        name: 'Last year',
        data: [44, 55, 57]
    }, {
        name: 'This year',
        data: [76, 85, 101]
    }],
    plotOptions: {
        bar: {
            horizontal: false,
            columnWidth: '55%',
            endingShape: 'rounded'
        },
    },
    dataLabels: {
        enabled: false
    },
    stroke: {
        show: true,
        width: 2,
        colors: ['transparent']
    },
    xaxis: {
        categories: ['May', 'June', 'July'],
    },
    yaxis: {
        title: {
            text: '$ (thousands)'
        }
    },
    tooltip: {
        y: {
            formatter: function (val) {
                return "$ " + val + " thousands"
            }
        }
    }
}
var chart = new ApexCharts(document.querySelector("#CashFlow"), options3);

chart.render();
options4 = {
    chart: {
        width: 500,
        height: 300,
        type: 'bar',
    },
    series: [{
        data: [{
            x: 'category A',
            y: 10
        }, {
            x: 'category B',
            y: 18
        }, {
            x: 'category C',
            y: 13
        }, {
            x: 'category A',
            y: 10
        }, {
            x: 'category B',
            y: 18
        }, {
            x: 'category C',
            y: 13
        }]
    }]
}
var chart = new ApexCharts(document.querySelector("#Products"), options3);

chart.render();



var tracking = {
    series: [
        {
            name: "High - 2023",
            data: [28, 29, 33, 36, 32, 32, 33]
        },
        {
            name: "Low - 2022",
            data: [12, 11, 14, 18, 17, 13, 13]
        }
    ],
    chart: {
        height: 350,
        type: 'line',
        dropShadow: {
            enabled: true,
            color: '#000',
            top: 18,
            left: 7,
            blur: 10,
            opacity: 0.2
        },
        toolbar: {
            show: false
        }
    },
    colors: ['#77B6EA', '#545454'],
    dataLabels: {
        enabled: true,
    },
    stroke: {
        curve: 'smooth'
    },
    title: {
        text: 'Average High & Low Temperature',
        align: 'left'
    },
    grid: {
        borderColor: '#e7e7e7',
        row: {
            colors: ['#f3f3f3', 'transparent'], // takes an array which will be repeated on columns
            opacity: 0.5
        },
    },
    markers: {
        size: 1
    },
    xaxis: {
        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
        title: {
            text: 'Month'
        }
    },
    yaxis: {
        title: {
            text: 'Temperature'
        },
        min: 5,
        max: 40
    },
    legend: {
        position: 'top',
        horizontalAlign: 'right',
        floating: true,
        offsetY: -25,
        offsetX: -5
    }
};

var chart = new ApexCharts(document.querySelector("#YearTrack"), tracking);

chart.render();
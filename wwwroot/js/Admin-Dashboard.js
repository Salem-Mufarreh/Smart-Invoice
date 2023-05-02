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
        width: 500,
        height:300,
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

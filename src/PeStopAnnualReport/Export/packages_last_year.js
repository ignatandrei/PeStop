
//this is the script
function DisplayPackagesStackLine(idDiv) 
{
    var myChart = echarts.init(document.getElementById(idDiv));

option = {
  title: {
    text: 'Packages 2025'
  },
  tooltip: {
    trigger: 'axis'
  },
  legend: {
    data: ['nr_pac_buc','nr_pac_tulcea','nr_pac_vaslui']
  },
  grid: {
    left: '3%',
    right: '4%',
    bottom: '3%',
    containLabel: true
  },
  toolbox: {
    feature: {
      saveAsImage: {}
    }
  },
  xAxis: {
    type: 'category',
    boundaryGap: false,
    data: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul','Aug','Sep','Oct','Nov','Dec']
  },
  yAxis: {
    type: 'value'
  },
  series: [


        {
        name: 'nr_pac_buc',
        type: 'bar',
        stack: 'Total',
        data: [574,550,0,0,602,551,607,0,0,0,0,0,0]
        },

    
        {
        name: 'nr_pac_tulcea',
        type: 'bar',
        stack: 'Total',
        data: [48,48,48,48,48,48,48,48,48,48,48,48,0]
        },

    
        {
        name: 'nr_pac_vaslui',
        type: 'bar',
        stack: 'Total',
        data: [50,50,50,50,50,50,50,50,0,50,50,50,0]
        },

    
  ]
};

      myChart.setOption(option);

      }
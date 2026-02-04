
//this is the script to display for voluntari with number cols 
function DisplayVoluntari(idDiv) 
{
    var myChart = echarts.init(document.getElementById(idDiv));

var option = {
  title: {
    text: 'Voluntari'
  },
  
  legend: {
    'show':true,
    'top':'top'
  },
  tooltip: {},
  dataset: {
    source: [
      ['voluntari', 'voluntari_bucuresti', 'voluntari_tulcea', 'voluntari_vaslui' ],

        ['2025', 25, 2, 2],

    
        ['2024', 20, 2, 0],

    
        ['2023', 18, 0, 0],

    
    ]
  },
  xAxis: [
    { type: 'category', gridIndex: 0 },
    { type: 'category', gridIndex: 1 }
  ],
  yAxis: [{ gridIndex: 0 }, { gridIndex: 1 }],
  grid: [{ bottom: '55%' }, { top: '55%' }],
  series: [
    // These series are in the first grid.

            { type: 'bar', seriesLayoutBy: 'row' },
        
            { type: 'bar', seriesLayoutBy: 'row' },
        
            { type: 'bar', seriesLayoutBy: 'row' },
            // These series are in the second grid.

            { type: 'bar', xAxisIndex: 1, yAxisIndex: 1 },
        
            { type: 'bar', xAxisIndex: 1, yAxisIndex: 1 },
        
            { type: 'bar', xAxisIndex: 1, yAxisIndex: 1 },
          ]
};
      myChart.setOption(option);

}

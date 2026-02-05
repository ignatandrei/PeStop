

//this is the script to display for radar tematics
function DisplayCursuriRadar(idDiv)
{
    var myChart = echarts.init(document.getElementById(idDiv));

    var option = {
  title: {
    text: 'Cursuri Radar Chart'
  },
  legend: {
    data: ['2024','2025'],
    show: true
  },
  radar: {
    // shape: 'circle',
    indicator: [


        { name: 'corp_consimtamant_minute'},
        { name: 'corp_consimtamant_numar'},
    
        { name: 'educatie_menstruala_minute'},
        { name: 'educatie_menstruala_numar'},
    
    ]
  },
  series: [
    {
      name: 'Tematics + Year',
      type: 'radar',
      data: [


        {
        value: [195,630,150,540],
        name: 'Allocated Budget'
        },
    
        {
        value: [360,2880,632,1890],
        name: 'Allocated Budget'
        },
    
      ]
    }
  ]
};


      myChart.setOption(option);
}
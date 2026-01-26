// Animate numbers when page loads
function animateNumbers() {
    const numbers = document.querySelectorAll('.metric-number');
    
    numbers.forEach(number => {
        const target = parseInt(number.getAttribute('data-value'));
        let current = 0;
        const increment = target / 50;
        
        const timer = setInterval(() => {
            current += increment;
            if (current >= target) {
                number.textContent = target.toLocaleString('ro-RO');
                clearInterval(timer);
            } else {
                number.textContent = Math.floor(current).toLocaleString('ro-RO');
            }
        }, 30);
    });
}

// Initialize charts
function initCharts() {
    // Distribution by Regions Chart
    const distributionCtx = document.getElementById('distributionChart').getContext('2d');
    new Chart(distributionCtx, {
        type: 'doughnut',
        data: {
            labels: ['București', 'Cluj', 'Iași', 'Constanța', 'Timișoara', 'Brasov', 'Alte regiuni'],
            datasets: [{
                data: [2145, 1876, 1654, 1234, 987, 854, 1797],
                backgroundColor: [
                    '#667eea',
                    '#764ba2',
                    '#f093fb',
                    '#4facfe',
                    '#00f2fe',
                    '#43e97b',
                    '#fa709a'
                ],
                borderColor: '#fff',
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'bottom'
                }
            }
        }
    });

    // Progress Chart
    const progressCtx = document.getElementById('progressChart').getContext('2d');
    new Chart(progressCtx, {
        type: 'line',
        data: {
            labels: ['Ian', 'Feb', 'Mar', 'Apr', 'Mai', 'Iun', 'Iul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [{
                label: 'Pachete Distribuite',
                data: [645, 789, 734, 892, 1024, 987, 1134, 1245, 1089, 1456, 1678, 1799],
                borderColor: '#667eea',
                backgroundColor: 'rgba(102, 126, 234, 0.1)',
                borderWidth: 3,
                fill: true,
                tension: 0.4,
                pointRadius: 5,
                pointBackgroundColor: '#667eea',
                pointBorderColor: '#fff',
                pointBorderWidth: 2
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    display: true
                }
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });

    // Activities Chart
    const activitiesCtx = document.getElementById('activitiesChart').getContext('2d');
    new Chart(activitiesCtx, {
        type: 'bar',
        data: {
            labels: ['Distribuție Directă', 'Cursuri Educație', 'Workshop', 'Voluntariat', 'Consultații'],
            datasets: [{
                label: 'Activități',
                data: [8547, 156, 234, 487, 892],
                backgroundColor: [
                    '#667eea',
                    '#764ba2',
                    '#f093fb',
                    '#4facfe',
                    '#43e97b'
                ],
                borderColor: '#fff',
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}

// Run when page loads
document.addEventListener('DOMContentLoaded', () => {
    animateNumbers();
    initCharts();
});

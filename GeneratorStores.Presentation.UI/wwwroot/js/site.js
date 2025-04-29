window.wishlist = {
    get: () => JSON.parse(localStorage.getItem("wishlist") || "[]"),
    count: () => JSON.parse(localStorage.getItem("wishlist") || "[]").length,
    add: (id) => {
        const set = new Set(window.wishlist.get());
        set.add(id);
        localStorage.setItem("wishlist", JSON.stringify([...set]));
    },
    remove: (id) => {
        const set = new Set(window.wishlist.get());
        set.delete(id);
        localStorage.setItem("wishlist", JSON.stringify([...set]));
    },
    clear: () => localStorage.removeItem("wishlist")
};

    window.renderSalesLineChart = (labels, data) => {
        var ctx = document.getElementById('salesChart').getContext('2d');
        new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Sales ($)',
                    data: data,
                    fill: true,
                    borderColor: '#6366F1',
                    backgroundColor: 'rgba(99, 102, 241, 0.2)',
                    tension: 0.4,
                    pointBackgroundColor: '#6366F1',
                    pointBorderColor: '#fff',
                    pointHoverBackgroundColor: '#fff',
                    pointHoverBorderColor: '#6366F1'
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        display: true,
                        position: 'top'
                    },
                    tooltip: {
                        mode: 'index',
                        intersect: false
                    }
                },
                scales: {
                    x: {
                        title: {
                            display: true,
                            text: 'Date'
                        }
                    },
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'Sales ($)'
                        }
                    }
                }
            }
        });
    }

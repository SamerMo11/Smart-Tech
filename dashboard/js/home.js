import { displayToast } from "./main.js";

function drawTopStatsChart(chardId, data, bg) {
  var options = {
    series: [
      {
        name: "series1",
        data: data,
      },
    ],
    chart: {
      height: 150,
      type: "area",
      toolbar: {
        show: false, // Disable toolbar
      },
      animations: {
        enabled: false, // Disable animations for consistency
      },
      offsetX: 0,
      offsetY: 0,
      background: "transparent",
    },
    colors: [bg], // Set the chart line color to orange
    fill: {
      type: "solid",
      color: bg, // Set the fill color to a solid dark background
      opacity: 0.1, //
    },
    dataLabels: {
      enabled: false,
    },
    stroke: {
      curve: "smooth",
      width: 2,
    },
    xaxis: {
      axisBorder: {
        show: false,
      },
      axisTicks: {
        show: false,
      },
      labels: {
        show: false,
      },
    },
    yaxis: {
      axisBorder: {
        show: false,
      },
      axisTicks: {
        show: false,
      },
      labels: {
        show: false,
      },
      min: 0, // Ensure the chart starts at the very bottom
    },
    grid: {
      show: false,
      padding: {
        top: 0,
        right: 0,
        bottom: 0,
        left: 0,
      },
    },
    tooltip: {
      enabled: false,
    },
  };

  var chart = new ApexCharts(document.querySelector("#" + chardId), options);
  chart.render();
}

drawTopStatsChart(
  "total-sales-chart",
  [31, 40, 28, 51, 42, 109, 100],
  "#f27d16"
);
drawTopStatsChart(
  "total-orders-chart",
  [331, 440, 285, 551, 426, 109, 530],
  "#f22e52"
);
drawTopStatsChart(
  "total-earnings-chart",
  [3431, 7440, 9285, 5651, 4326, 1609, 5030],
  "#1edf71"
);

function drawOrderAnalytics(series) {
  var options = {
    chart: {
      type: "line",
      height: 350,
      toolbar: {
        show: false, // Hides the toolbar
      },
      animations: {
        enabled: false, // Disable animations for consistency
      },
    },
    series: [...series],
    grid: {
      show: false,
      padding: {
        left: 0,
        right: 0, // Removes padding on both sides
      },
    },
    xaxis: {
      // categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul"], // Month labels
      title: {
        text: "Month",
      },
      range: undefined, // Automatically adjusts based on data
      axisBorder: {
        show: false,
      },
      axisTicks: {
        show: false,
      },
      labels: {
        style: {
          colors: "#d1d0d0", // Sets the color of x-axis labels
          fontSize: "12px", // Optional: Adjust font size
        },
      },
      offsetY: 5,
    },
    yaxis: {
      title: {
        text: "Orders",
      },
      labels: {
        style: {
          colors: "#d1d0d0", // Sets the color of x-axis labels
          fontSize: "12px", // Optional: Adjust font size
        },
        offsetX: -20,
      },
      offsetX: -10,
    },
    stroke: {
      curve: "smooth", // Smooth line style
      width: 2,
    },
    tooltip: {
      enabled: true,
      x: {
        format: "dd MMM yyyy",
      },
    },
    colors: ["var(--color-green)", "var(--color-red)"], // Colors for the lines
    legend: {
      show: false,
    },
  };

  // Render the chart
  var chart = new ApexCharts(document.querySelector("#order--ana"), options);
  chart.render();
}
drawOrderAnalytics([
  {
    name: "Online Payment",
    data: [20, 30, 45, 50, 49, 60, 70, 20, 30, 45, 50, 49, 60, 70], // Sample data
  },
  {
    name: "Offline Payment",
    data: [10, 20, 35, 40, 39, 50, 60, 23, 20, 20, 45, 20, 42, 46], // Sample data
  },
]);

function drawOrdersByCategoryChart(labels, values) {
  var options = {
    series: values,
    chart: {
      height: 260,
      type: "polarArea",
    },
    labels: labels,
    fill: {
      opacity: 1,
    },
    stroke: {
      width: 1,
      colors: undefined,
    },
    yaxis: {
      show: false,
    },
    legend: {
      position: "bottom",
      labels: {
        colors: "var(--color-light-gray)",
      },
      markers: {
        offsetX: -5,
      },
    },
    plotOptions: {
      polarArea: {
        rings: {
          strokeWidth: 0,
        },
        spokes: {
          strokeWidth: 0,
        },
      },
    },
    theme: {
      monochrome: {
        enabled: false,
        shadeTo: "light",
        shadeIntensity: 0.6,
      },
    },
  };

  var chart = new ApexCharts(
    document.querySelector("#ordersByCategory"),
    options
  );
  chart.render();
}
drawOrdersByCategoryChart(
  ["Cameras", "Headphones", "Keyboard", "Laptop", "Mouse"],
  [42, 47, 52, 58, 65]
);

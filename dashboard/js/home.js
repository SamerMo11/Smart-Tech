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

function drawOrderAnalytics(series, xAxis) {
  console.log(series);
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
      categories: xAxis, // Month labels
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
async function getData() {
  try {
    const req = await fetch("./js/home.json");

    if (!req.ok) {
      throw new Error("Can't Load Data");
    }
    return await req.json();
  } catch (e) {
    throw e;
  }
}
getData().then((data) => {
  console.log(data);
  displayProductList(data.productList);
  drawOrderAnalytics(
    orderAnalyticsSeries(data.orderAnalytics),
    orderAnalyticsXAxis(data.orderAnalytics)
  );

  console.log(orderAnalyticsXAxis(data.orderAnalytics));
});
function orderAnalyticsXAxis(orderAnalytics) {
  return orderAnalytics.map((obj) => obj.month);
}
function orderAnalyticsSeries(arr) {
  const obj = {};
  arr.map((e) => {
    for (const [k, v] of Object.entries(e)) {
      if (k == "month") continue;
      if (obj[k]) {
        obj[k].push(v);
      } else obj[k] = [v];
    }
  });
  console.log(
    Object.entries(obj).map((ele) => {
      return { name: ele[0], data: ele[1] };
    })
  );
  return Object.entries(obj).map((ele) => {
    return { name: ele[0], data: ele[1] };
  });
}
function displayProductList(productList) {
  const productsDiv = document.querySelector(".products--teble");
  console.log(productsDiv);
  let html = "";
  for (let i = 0; i < productList.length; i++) {
    const { variantId, name, price, status, qtyLeft } = productList[i];
    html += `
    <ul>
              <li>${variantId}</li>
              <li>${name}</li>
              <li>$${price}</li>
              <li class="status in--stuck"><span>${status}</span></li>
              <li>${qtyLeft}</li>
            </ul>`;
  }
  productsDiv.insertAdjacentHTML("beforeend", html);
}

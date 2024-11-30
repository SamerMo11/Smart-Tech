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
    colors: ["var(--color-green)", "var(--color-orange)", "var(--color-red)"], // Colors for the lines
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
  displayProductList(data.productList);
  drawOrdersByCategoryChart(
    data.ordersByCategory.names,
    data.ordersByCategory.numOfSalles
  );
  console.log(data.names, data.numOfSalles);
  updateTopStats([
    data.totalSales,
    data.totalRefundOrders,
    data.totalApprovedOrders,
  ]);
  drawOrderAnalytics(
    orderAnalyticsSeries(data.orderAnalytics),
    orderAnalyticsXAxis(data.orderAnalytics)
  );
  displayMostSelling(data.mostSelling);
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
function updateTopStats([salesVal, ordersVal, earningsVal]) {
  const arr = [salesVal, ordersVal, earningsVal];
  const [totalSales, totalOrders, totalEarnings] = document.querySelectorAll(
    ".stats .stats--value"
  );
  totalSales.innerHTML = salesVal.value + "$";
  totalOrders.innerHTML = ordersVal.value;
  totalEarnings.innerHTML = earningsVal.value;

  drawTopStatsChart("total-sales-chart", salesVal.days, "#f27d16");
  drawTopStatsChart("total-orders-chart", ordersVal.days, "#f22e52");
  drawTopStatsChart("total-earnings-chart", earningsVal.days, "#1edf71");

  // Update Stats analysis
  const statsAnalysis = document.querySelectorAll(".stats--head > div");
  statsAnalysis.forEach((ele, i) => {
    ele.innerHTML = ` 
              <p class="stats--value">${arr[i].value}$</p>
              <div class="stats--analysis green">
                <p class=""><i class="fa-solid fa-arrow-trend-up" aria-hidden="true"></i> 10.2</p>
                <p class="stat">+1.1% this week</p>
              </div>
            `;
  });
  console.log(statsAnalysis);
}
function getCategoryChartData(ordersByCategory) {
  const arr = [[], []];
  ordersByCategory.map((e, i) => {
    arr[0].push(e.name);
    arr[1].push(e.numOfSalles);
  });
  return arr;
}
function displayMostSelling(arr) {
  let html = "";
  const table = document.querySelector(".top--sales .products");
  table.innerHTML = "";
  arr.map((ele) => {
    const { variantId, productName, saledQuantity, totalSales } = ele;
    html += `
    <div>
              <img src="./images/product.png" alt="Product image">
              <p class="product--title">
                ${productName} <span class="sells--value">Sell:${saledQuantity}</span>
              </p>
              <p class="product--price">$${totalSales}</p>
            </div>
            `;
  });
  table.insertAdjacentHTML("beforeend", html);
}

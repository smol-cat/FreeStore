<template>
  <div class="item">
    <div class="mainItemInfo">
      <div class="price">
        <p>{{ item.price.toFixed(2) }} €</p>
      </div>
      <div class="itemTitle">
        <a :href="'/categories/' + item.category.id + '/items/' + item.id">{{ item.title }}</a>
      </div>
    </div>
    <div class="itemBottom">
      <a>Paskelbė: {{ item.account.name + " " + item.account.last_name }}</a>
      <p>{{ item.state.name }}</p>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    item: Object
  }
}
</script>

<style>
.item {
  text-align: left;
  max-width: 500px;
  background-color: #91a1a1;
  margin: auto;
  padding: 10px;
  margin-bottom: 10px;
  border-radius: 10px;
}

.itemTitle a {
  padding: 0;
  margin: 0;
  color: rgb(42, 70, 68);
}

.price {
  float: right;
}

.mainItemInfo{
  margin-bottom: 20px;
}

.itemBottom{
  font-size: 0.8em;
}
</style>
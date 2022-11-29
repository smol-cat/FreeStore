<template>
  <button class="refButton" type="submit">{{ text }}</button>
</template>

<script>
export default {
  props: {
    text: String
  }
}
</script>

<style>
.refButton button {
  max-width: none;
  font-family: inherit;
}

</style>

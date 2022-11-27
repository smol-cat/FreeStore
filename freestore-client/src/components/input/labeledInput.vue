<template>
  <input :id="_id" :type="_type" :placeholder="_placeholder" :required="_required" />
</template>

<script>

export default {
  name: 'labeledInput',
  props: {
    _id: String,
    _placeholder: String,
    _type: String,
    _required: Boolean
  }
}
</script>

<style>
  input{
    width: 300px;
    margin: 5px;
    padding: 10px;
    border-radius: 10px;
    border: none
  }
</style>

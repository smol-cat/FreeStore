<template>
    <input class="blockButton" type="button" :value="this.text" v-on:click="onClick()" />
</template>

<script>
export default {
    props: {
        onClick: Function,
        text: String
    },
}
</script>

<style>
.blockButton {
    margin: 0 auto;
    padding: 5px;
    border: none;
    border-radius: 10px;
    color: #a1b6ca;
    background-color: rgb(42, 70, 68);
    cursor: pointer;
}

.blockButton:hover {
    background-color: #57a1a1;
    color: white;
}
</style>
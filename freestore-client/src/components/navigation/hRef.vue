<template>
    <div>
        <a class="refButton" :href="link">{{ label }}</a>
    </div>
</template>

<script>
export default {
    name: "hRef",
    props: {
        label: String,
        link: String,
    }
}
</script>

<style>
</style>
<template>
    <button class="commentButton" type="submit">{{ text }}</button>
</template>

<script>
export default {
    props: {
        text: String
    }
}
</script>

<style>
.commentButton {
    padding: 5px;
    border: none;
    border-radius: 10px;
    color: #a1b6ca;
    background-color: rgb(42, 70, 68);
}

.commentButton:hover {
    background-color: #57a1a1;
    color: white;
}
</style>
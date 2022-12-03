<template>
    <div class="item comment">
        <p class="postDate">{{ this.getCustomDate(comment.date_created) }}</p>
        <p class="author">{{ comment.account.name + " " + comment.account.last_name }}</p>
        <p>{{ comment.text }}</p>
    </div>
</template>

<script>
export default {
    props: {
        comment: Object
    },
    methods: {
        getCustomDate(dateString) {
            var dateTime = new Date(dateString)
            return Intl.DateTimeFormat("lt", {
                dateStyle: "short",
                timeStyle: "short"
            }).format(dateTime)
        }
    }
}
</script>

<style>
.comment {
    max-width: 400px;
    color: rgb(44, 42, 70);
}

.comment hr {
    border: 1px solid rgb(42, 70, 68)
}

.author {
    font-size: 0.8em;
}

.postDate {
    font-size: 0.8em;
    float: right;
}

.author {
    margin-bottom: 10px;
}
</style>
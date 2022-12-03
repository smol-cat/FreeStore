<template>
    <form class="item comment" @submit.prevent="onSubmit">
        <p>Naujas komentaras</p>
        <a class="errorMessage"></a>
        <div class="submitComment">
            <commentSubmit :text="'Skelbti'" />
        </div>
        <commentInput />
    </form>
</template>

<script>
import commentSubmit from './commentSubmit.vue'
import commentInput from './commentInput.vue'

export default {
    components: {
        commentInput,
        commentSubmit,
    },
    methods: {
        async onSubmit(submitEvent) {
            var body = {
                "text": submitEvent.target.elements.text.value,
            }

            var endpoints = location.pathname.split('/')
            var response = await this.performRequest(`/categories/${endpoints[2]}/items/${endpoints[4]}/comments`, "POST", body)
            if(response.success){
                window.location.reload()
            }
            else{
                console.log(document.getElementsByClassName("errorMessage")[0].text)
                document.getElementsByClassName("errorMessage")[0].text = response.body.message
            }
        }
    }
}
</script>

<style>
.comment p {
    max-width: 300px;
}


.submitComment {
    float: right;
}
</style>
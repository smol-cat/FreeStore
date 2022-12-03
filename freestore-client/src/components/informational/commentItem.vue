<template>
    <div v-if="!this.editMode" class="item comment">
        <p class="postDate">{{ this.getCustomDate(comment.date_created) }}</p>
        <p class="author">{{ comment.account.name + " " + comment.account.last_name }}</p>

        <div class="commentText">
            <p>{{ comment.text }}</p>
        </div>
        <div class="buttonRow">
            <div v-if="this.ableToEdit()" style="margin-right: 10px">
                <block-button :text="'redaguoti'" :onClick="() => editMode = true" />
            </div>
            <div v-if="this.ableToDelete()">
                <block-button :text="'šalinti'" :onClick="() => this.$emit('triggerModal', 'Tikrai norite pašalinti šį komentarą?', deleteComment)" />
            </div>
        </div>
        <div v-if="this.ableToEdit() || this.ableToDelete()" class="extraSpace" />
    </div>

    <div v-else class="item comment">
        <p class="postDate">{{ this.getCustomDate(comment.date_created) }}</p>
        <p class="author">{{ comment.account.name + " " + comment.account.last_name }}</p>
        <p class="errorMessage"></p>
        <form @submit.prevent="onSubmit">
            <div>
                <textarea id="text" class="commentInput" :value="comment.text" />
            </div>
            <div class="buttonRow">
                <div>
                    <block-button style="margin-right: 10px" :text="'atšaukti'" :onClick="() => editMode = false" />
                </div>
                <div>
                    <block-button :text="'išsaugoti'" :isSubmit="true" />
                </div>
            </div>
        </form>
        <div class="extraSpace" />
    </div>
</template>

<script>
import blockButton from '../input/blockButton.vue'

export default {
    data() {
        return {
            editMode: false,
        }
    },
    props: {
        comment: Object
    },
    components: {
        blockButton
    },
    emits: [ 'triggerModal' ],
    methods: {
        getCustomDate(dateString) {
            var dateTime = new Date(dateString)
            return Intl.DateTimeFormat("lt", {
                dateStyle: "short",
                timeStyle: "short"
            }).format(dateTime)
        },
        ableToDelete() {
            return this.comment.account.id.toString() === sessionStorage["userId"] || sessionStorage["userLevel"] === '1'
        },
        ableToEdit() {
            return this.comment.account.id.toString() === sessionStorage["userId"]
        },
        async onSubmit(submitEvent){
            var errorMessage = document.getElementsByClassName("errorMessage")[0]
            errorMessage.text = ''

            var endpoints = location.pathname.split('/')
            var body = {
                text: submitEvent.target.elements.text.value
            }
            var response = await this.performRequest(`/categories/${endpoints[2]}/items/${endpoints[4]}/comments/${this.comment.id}`, 'PUT', body)
            if(response.success){
                location.reload()
            }
            else{
                errorMessage.text = response.body?.message || "Įvyko klaida" 
            }
        },
        async deleteComment(){
            var errorMessage = document.getElementsByClassName("errorMessage")[0]
            errorMessage.text = ''
            var endpoints = location.pathname.split('/')

            var response = await this.performRequest(`/categories/${endpoints[2]}/items/${endpoints[4]}/comments/${this.comment.id}`, 'DELETE')
            if(response.success){
                location.reload()
            }
            else{
                errorMessage.text = response.body?.message || "Įvyko klaida" 
            }
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

.commentText {
    margin-top: 20px;
}

.extraSpace {
    height: 30px;
}
</style>
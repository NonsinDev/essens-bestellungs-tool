
<template>
    <div class="card flex justify-center">
        <IftaLabel>
            <label for="username">Username</label>
            <InputText id="username" v-model="value" variant="filled" />
            
        </IftaLabel>
    </div>
</template>

<script setup>
import { ref } from 'vue';
import { InputText } from 'primevue';

const value = ref(null);
</script>

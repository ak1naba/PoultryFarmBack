<template>
    <div>
        Main view
        {{ mess }}
    </div>
</template>

<script>
export default {
    name: 'MainView',
    data() {
        return {
            mess: 'Main view mess'
        };
    },
    methods:{
        getChickens(){
            axios.get('http://localhost:8080/api/chicken/')
            .then(res => {
                console.log(res);
            })
            .catch(err=>{
                console.log(err);
                
            })
        }
    }, 
    mounted(){
        this.getChickens()
    }
    
};
</script>

<style scoped>
</style>

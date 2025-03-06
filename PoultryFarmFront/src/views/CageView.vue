<template>
  <BaseLayout>
    <div v-if="!loading">
      <transition name="fade" mode="out-in">
        <form v-if="creation" @submit.prevent="storeCage()" class="form-create">
          <select v-model="cageCreation.employeeId" required>
            <option value=""></option>
            <option v-for="employee in employees" :key="employee.id" :value="employee.id">
              {{ employee.fullName }}
            </option>
          </select>
          <div>
            <button class="btn-primary" type="submit"> Добавить </button>
          </div>
          <div class="form-create__errors" v-if="errorCreation">
            <ul>
              <li v-for="(error, index) in errorCreation" :key="index">
                {{ error }}
              </li>
            </ul>
          </div>
        </form>
      </transition>
      <div class="cage">
        <div class="cage-action">
          <button v-if="!creation" class="btn-primary" @click="creation = true"> Добавить </button>
          <button v-else class="btn-secondary" @click="creation = false"> Отмена </button>
        </div>
        <div class="cage-item" v-for="cage in cages" :key="cage.id">
          <slot v-if="!cage.editing">
            <div class="cage-item__character">
              <span class="cage-item__character-label"> Номер </span> {{ cage.id }}
            </div>
            <div class="cage-item__character">
              <span class="cage-item__character-label"> Занятость </span> {{ cage.isOccupied ? 'Занята' : 'Свободна' }}
            </div>
            <div class="cage-item__character">
              <span class="cage-item__character-label"> Номер курицы </span> {{ cage.chickenId ? cage.chickenId : 'Свободна' }}
            </div>
            <div class="cage-item__character">
              <span class="cage-item__character-label"> Работник </span> {{ cage.employee.fullName }}
            </div>
            <button class="btn-secondary" @click="toggleEdit(cage)"> Редактировать </button>
          </slot>
          <form v-else @submit.prevent="updateCage(cage)" class="form-update">
            <div class="form-update__title"></div>Редактирование клетки {{cage.id}}
            <select v-model="cage.employeeId" required>
              <option v-for="employee in employees" :key="employee.id" :value="employee.id">
                {{ employee.fullName }}
              </option>
            </select>
            <div class="form-update__buttons">
              <button class="btn-primary" type="submit"> Сохранить </button>
              <button class="btn-secondary" type="button" @click="toggleEdit(cage)"> Отмена </button>
              <button v-if="!cage.isOccupied" class="btn-danger" type="button" @click.prevent="deleteCage(cage)"> Удалить </button>
            </div>
            <div class="form-update__errors" v-if="errorUpdating">
              <ul>
                <li v-for="(error, index) in errorUpdating" :key="index">
                  {{ error }}
                </li>
              </ul>
            </div>
          </form>
        </div>
      </div>
    </div>
    <div v-else>
      Loading...
    </div>
  </BaseLayout>
</template>

<script>
import BaseLayout from '@/layouts/BaseLayout.vue';
import axios from 'axios';
import Swal from "sweetalert2";

export default {
  name: 'CageView',
  components: {
    BaseLayout
  },
  data() {
    return {
      loading: true,
      creation: false,
      cages: [],
      cageCreation: {
        employeeId: null,
      },
      employees: [],
      errorCreation: [],
      errorUpdating: [],
    };
  },
  methods: {
    getCages() {
      this.loading = true;
      axios.get('http://localhost:8080/api/cage')
          .then(res => {
            this.cages = res.data.map(cage => ({
              ...cage,
              editing: false
            }));
          })
          .catch(err => {
            console.log(err);
          })
          .finally(() => {
            this.loading = false;
          });
    },
    storeCage() {
      this.errorCreation = [];
      const cage = {
        employeeId: Number(this.cageCreation.employeeId)
      };
      axios.post('http://localhost:8080/api/cage', cage)
          .then(res => {
            this.getCages();
            this.creation = false;
          })
          .catch(err => {
            this.errorCreation = err.response.data.errors;
            console.log(this.errorCreation);
          });
    },
    toggleEdit(cage) {
      cage.editing = !cage.editing;
    },
    updateCage(cage) {
      this.errorsUpdating = [];
      const cageDTO = {
        id: cage.id,
        employeeId: Number(cage.employeeId)
      };
      axios.put(`http://localhost:8080/api/cage/${cage.id}`, cageDTO)
          .then(res => {
            console.log(res.data);
            this.getCages();
            this.getEmployees();
            chicken.editing = false;
            this.errorsUpdating = [];
          })
          .catch(err => {
            this.errorUpdating = err.response.data.errors;
            console.log(this.errorUpdating);
          });
    },
    deleteCage(cage) {
      Swal.fire({
        title: 'Вы уверены?',
        text: 'Это действие нельзя отменить!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Да, удалить!',
        cancelButtonText: 'Отмена'
      }).then((result) => {
        if (result.isConfirmed) {
          axios.delete(`http://localhost:8080/api/cage/${cage.id}`)
              .then(() => {
                Swal.fire('Удалено!', 'Курица успешно удалена.', 'success');
                this.getCages();
              })
              .catch(err => {
                Swal.fire('Ошибка!', 'Не удалось удалить курицу.', 'error');
                console.log(err);
              });
        }
      });
    },
    getEmployees(){
      axios.get('http://localhost:8080/api/employee')
          .then(res => {
           this.employees = res.data
          })
          .catch(err => {
            console.log(err);
          })
    }
  },
  mounted() {
    this.getCages();
    this.getEmployees();
  }
};
</script>


<style lang="scss" scoped>
.form-create {
  display: flex;
  flex-direction: column;
  gap: 10px;

  margin-bottom: 25px;

  &__errors {
    color: #910303;
    font-weight: 600;
  }
}

.cage {
  display: flex;
  flex-direction: column;
  gap: 15px;

  &-item{
    width: 100%;

    padding: 10px;
    border-radius: 10px;
    border: 1px solid #000;

    display: flex;
    flex-direction: row;
    gap: 5px;

    &__character{
      display: flex;
      flex-direction: column;
      gap: 5px;

      width: 15%;

      font-size: 14px;

      &-label{
        font-size: 10px !important;
        color: gray;
      }
    }

    .form-update{
      width: 100%;
      display: flex;
      flex-direction: column;
      gap: 10px;

      margin-bottom: 25px;

      &__errors {
        padding-left: 25px;
        color: #910303;
        font-weight: 600;
      }

      &__buttons{
        display: flex;
        flex-direction: row;
        gap: 5px;
      }
    }
  }
}
</style>
  
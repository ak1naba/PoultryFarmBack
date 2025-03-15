<template>
  <BaseLayout>
    <div v-if="!loading">
      <transition name="fade" mode="out-in">
        <form v-if="creation" @submit.prevent="storeEmployee()" class="form-create">
          <input v-model="employeeCreation.fullName" type="text" placeholder="ФИО" required>
          <input v-model="employeeCreation.salary" type="number" step="0.1" placeholder="Зарплата" required>
          <div>
            <button class="btn-primary" type="submit"> Добавить </button>
          </div>
          <div class="form-create__errors" v-if="errorsCreation">
            <ul>
              <li v-for="(error, index) in errorsCreation" :key="index">
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
        <div class="cage-item" v-for="employee in employees" :key="employee.id">
          <slot v-if="!employee.editing">
            <div class="cage-item__character">
              <span class="cage-item__character-label"> Номер </span> {{ employee.id }}
            </div>
            <div class="cage-item__character">
              <span class="cage-item__character-label"> Имя </span> {{ employee.fullName}}
            </div>
            <div class="cage-item__character">
              <span class="cage-item__character-label"> Зарплата </span> {{ employee.salary }}
            </div>
            <div class="cage-item__character">
              <span class="cage-item__character-label"> Клетки </span>
              {{ joinCages(employee.cages) }}
            </div>
            <button class="btn-secondary" @click="toggleEdit(employee)"> Редактировать </button>
          </slot>
          <form v-else @submit.prevent="updateEmployee(employee)" class="form-update">
            <div class="form-update__title"></div>Редактирование сотрудника {{employee.id}} - {{employee.fullName}}
            <input v-model="employee.fullName" type="text" placeholder="ФИО" required>
            <input v-model="employee.salary" type="number" step="0.1" placeholder="Зарплата" required>
            <div class="form-update__buttons">
              <button class="btn-primary" type="submit"> Сохранить </button>
              <button class="btn-secondary" type="button" @click.prevent="toggleEdit(employee)"> Отмена </button>
              <button v-if="employee.cages.length == 0" class="btn-danger" type="button" @click.prevent="deleteEmployee(employee)"> Удалить </button>
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
  name: 'EmployeeView',
  components: {
    BaseLayout
  },
  data() {
    return {
      loading: true,
      creation: false,
      employeeCreation: {
        fullName: null,
        salary: null,
      },
      employees: [],
      errorsCreation: [],
      errorsUpdating: [],
    };
  },
  methods: {
    getEmployees() {
      this.loading = true;
      axios.get('http://localhost:8080/api/employee')
          .then(res => {
            this.employees = res.data.map(employee => ({
              ...employee,
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
    joinCages(cages){
      const ids = cages.map(cage => cage.id);
      return ids.join(', ');
    },
    storeEmployee() {
      this.errorCreation = [];
      const employee = {
        fullName: this.employeeCreation.fullName,
        salary: this.employeeCreation.salary,
      };
      axios.post('http://localhost:8080/api/employee', employee)
          .then(res => {
            console.log(res)
            this.creation = false;
          })
          .catch(err => {
            this.errorCreation = err.response.data.errors;
            console.log(this.errorCreation);
          });
    },
    toggleEdit(employee) {
      employee.editing = !employee.editing;
    },
    updateEmployee(employee) {
      this.errorsUpdating = [];
      const employeeDTO = {
        id: employee.id,
        fullName: employee.fullName,
        salary: Number(employee.salary)
      };
      axios.put(`http://localhost:8080/api/employee/${employee.id}`, employeeDTO)
          .then(res => {
            console.log(res.data);
            this.getEmployees();
            employee.editing = false;
            this.errorsUpdating = [];
          })
          .catch(err => {
            this.errorUpdating = err.response.data.errors;
            console.log(this.errorUpdating);
          });
    },
    deleteEmployee(employee) {
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
          axios.delete(`http://localhost:8080/api/employee/${employee.id}`)
              .then(() => {
                Swal.fire('Удалено!', 'Сотрудник успешно удален.', 'success');
                this.getEmployees();
              })
              .catch(err => {
                Swal.fire('Ошибка!', 'Не удалось удалить курицу.', 'error');
                console.log(err);
              });
        }
      });
    },

  },
  mounted() {
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
  
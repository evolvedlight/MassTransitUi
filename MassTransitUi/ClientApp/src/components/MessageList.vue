<template>
  <div class="table-responsive">
    <table class="table table-striped table-hover">
      <thead>
        <th>Id</th>
        <th>Queue</th>
        <th>MessageId</th>
        <th>Fault</th>
        <th>ErrorMessage</th>
        <th>Actions</th>
      </thead>
      <tbody>
        <tr v-for="message in filteredMessages" :key="message.Id" style="height:60px">
          <td>{{ message.Id }}</td>
          <td>{{ message.Queue }}</td>
          <td>{{ message.MessageId }}</td>
          <td>{{ message.Fault }}</td>
          <td>{{ message.ErrorMessage }}</td>
          <td>
            <div v-if="message.Status === 'new'">
              <a class="btn btn-primary" @click="retryMessage(message)">Retry</a>
              <a class="btn btn-primary ml-2" @click="deleteMessage(message)">Delete</a>
            </div>
            <div v-if="message.Status === 'inprogress'" class="alert alert-success p-2 m-0" role="alert">
              In Progress
            </div>
            <div v-if="message.Status === 'retried' || message.Status === 'deleted'" class="alert alert-success p-2 m-0" role="alert">
              Successfully {{ message.Status }}
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'MessageList',
  props: {
    filteredMessages: Array,
  },
  methods: {
    retryMessage(message) {
      message.Status = "retrying";
      console.log("retrying" + message.MessageId);

      axios.post('/api/message/' + message.Id + "/retry", {
      })
      .then(function (response) {
        message.Status = "retried"
        console.log(response);
      })
      .catch(function (error) {
        message.Status = "error on retry"
        console.log(error);
      });
    },
    deleteMessage(message) {
      message.Status = "deleting";

      axios.post('/api/message/' + message.Id + "/delete", {
      })
      .then(function (response) {
        message.Status = "deleted"
        console.log(response);
      })
      .catch(function (error) {
        message.Status = "error on delete"
        console.log(error);
      });
    },
  }
}
</script>

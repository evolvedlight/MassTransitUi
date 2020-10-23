<template>
  <div>
    <MessageList :messages="messages" />
  </div>
</template>

<script>
import MessageList from './components/MessageList.vue'
import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
  name: 'App',
  components: {
    MessageList
  },
  data() {
    return {
      messages: []
    }
  },
  created () {
    const connection = new HubConnectionBuilder()
      .withUrl('/errorQueueHub')
      .configureLogging(LogLevel.Information)
      .build()

    connection.on('NewErrorInQueue', (messageObj) => {
        console.log(messageObj);
        this.messages.push(JSON.parse(messageObj))
    })
 
    connection.start()
  }
}
</script>

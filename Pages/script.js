new Vue({
    el: "#app",

    data: {
        baseUrl: "http://localhost:5137",
        service: "Your Service Title",
        datetime: null,
        statistics: null
    },

    async mounted() {
        await this.sync();
    },

    methods: {
        async sync() {
            const now = new Date() // current date
            const months = ["January", "February", "March", "April", "May ", "June", "July", "August", "September", "October", "November", "December"];

            let date = [months[now.getMonth()], now.getDate(), now.getFullYear()].join(" ");
            let time = now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds();

            setTimeout(this.sync, 5000);
            await this.fetchStatistics()
            this.datetime = [date, time].join(" ");
        },

        async reset() {
            await fetch(`${this.baseUrl}/statistic/Reset`, {method: "PUT"})
            await this.fetchStatistics()
        },

        async fetchStatistics() {
            this.statistics = await (await fetch(`${this.baseUrl}/statistic/List`)).json()
        },

        secondToHumanReadable(seconds) {
            function numberEnding(number) {
                return (number > 1) ? "s" : "";
            }

            let temp = Math.floor(seconds);
            let years = Math.floor(temp / 31536000);
            if (years) {
                return years + " year" + numberEnding(years);
            }

            const days = Math.floor((temp %= 31536000) / 86400);
            if (days) {
                return days + " day" + numberEnding(days);
            }
            const hours = Math.floor((temp %= 86400) / 3600);
            if (hours) {
                return hours + " hour" + numberEnding(hours);
            }
            const minutes = Math.floor((temp %= 3600) / 60);
            if (minutes) {
                return minutes + " minute" + numberEnding(minutes);
            }
            const rSeconds = temp % 60;
            if (rSeconds) {
                return rSeconds + " second" + numberEnding(rSeconds);
            }
            return "less than a second"; //"just now" //or other string you like;
        },
    },

})
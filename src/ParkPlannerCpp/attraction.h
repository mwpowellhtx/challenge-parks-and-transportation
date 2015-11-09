#ifndef PARKPLANNER_ATTRACTION_H
#define PARKPLANNER_ATTRACTION_H

#include <map>
#include <vector>
#include <istream>

namespace challenges {

    namespace park {

        class attraction {
        private:

            std::map<int, int> _queue_times;

        public:

            attraction(std::vector<int> const & queue_times);

            attraction(attraction const & other);

            virtual ~attraction();

            bool try_get_queue_time(int time_minutes, int & queue_time);

            static std::vector<attraction> read_all(std::istream & is, int count);

        private:

            static attraction read_one(std::istream & is);
        };
    }
}

#endif //PARKPLANNER_ATTRACTION_H

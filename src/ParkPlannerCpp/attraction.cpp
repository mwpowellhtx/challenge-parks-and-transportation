#include "attraction.h"
#include "constants.h"

#include <string>

#include <common\parsers.h>
#include <common\strutil.h>

namespace challenges {

    namespace park {

        attraction::attraction(std::vector<int> const & queue_times)
            : _queue_times()
        {
            for (auto i = 0; i < static_cast<int>(queue_times.size()); i++)
                _queue_times.emplace(i, *(queue_times.begin() + i));
        }

        attraction::attraction(attraction const & other)
            : _queue_times(other._queue_times){
        }

        attraction::~attraction()
        {
            _queue_times.clear();
        }

        bool attraction::try_get_queue_time(int time_minutes, int & queue_time) {
            queue_time = 0;
            auto hour = time_minutes / constants::minutes_per_hour;
            if (_queue_times.find(hour) == _queue_times.end()) return false;
            queue_time = _queue_times.at(hour);
            return true;
        }

        attraction attraction::read_one(std::istream & is) {

            std::string line;

            getline(is, line);

            split splitter;

            std::vector<int> queue_times;

            for (auto const & x : splitter(line))
                queue_times.push_back(to_int(x));

            return attraction(queue_times);
        }

        std::vector<attraction> attraction::read_all(std::istream & is, int count) {

            std::vector<attraction> results;

            while (count-- > 0)
                results.push_back(read_one(is));

            return results;
        }
    }
}
#include "theme_park.h"
#include "constants.h"
#include "attraction.h"

#include <common\streamutil.h>

#include <string>

namespace challenges {

    namespace park {

        theme_park::theme_park(std::vector<attraction> const & attractions, int max_hours_per_day)
            : _max_hours_per_day(max_hours_per_day)
            , _attractions(attractions)
            , _guests()
        {
        }

        theme_park::~theme_park()
        {
        }

        int theme_park::max_hours_per_day() const {
            return _max_hours_per_day;
        }

        int theme_park::max_minutes_per_day() const {
            const int result = _max_hours_per_day*constants::minutes_per_hour;
            return result;
        }

        int theme_park::attraction_count() const {
            return static_cast<int>(_attractions.size());
        }

        attraction & theme_park::get_attraction(std::vector<attraction>::size_type index) {
            return *(_attractions.begin() + index);
        }

        std::shared_ptr<theme_park> theme_park::read(std::istream & is) {

            auto values = read_ints(is);

            auto attraction_count = values[0];
            auto max_hours_per_day = values[1];

            auto attractions = attraction::read_all(is, attraction_count);

            auto result = std::make_shared<theme_park>(attractions, max_hours_per_day);

            auto query_count = read_ints(is)[0];

            result->_guests = guest::read_all(is, query_count, result.get());

            return result;
        }
    }
}

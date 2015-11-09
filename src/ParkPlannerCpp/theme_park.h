#ifndef PARKPLANNER_THEME_PARK_H
#define PARKPLANNER_THEME_PARK_H

#include "attraction.h"
#include "guest.h"

#include <vector>
#include <istream>
#include <memory>

namespace challenges {

    namespace park {

        class theme_park {
        private:

            int _max_hours_per_day;

            std::vector<attraction> _attractions;

        public:

            theme_park(std::vector<attraction> const & attractions, int max_hours_per_day);

            ~theme_park();

            std::vector<guest> _guests;

            int max_hours_per_day() const;

            int max_minutes_per_day() const;

            int attraction_count() const;

            attraction & get_attraction(std::vector<attraction>::size_type index);

            static std::shared_ptr<theme_park> read(std::istream & is);
        };
    }
}

#endif //PARKPLANNER_THEME_PARK_H

#include "visit.h"
#include "constants.h"

#include <common\vectorutil.h>

#include <algorithm>
#include <limits>

namespace challenges {

    namespace park {

        const int visit::_impossible = std::numeric_limits<int>::max();


        visit::visit(guest * p_guest, theme_park * p_theme_park, std::vector<int> const & desired)
            : _p_guest(p_guest)
            , _p_theme_park(p_theme_park)
            , _desired(desired)
            , _sp_time_minutes() {
        }

        visit::visit(visit const & other)
            : _p_guest(other._p_guest)
            , _p_theme_park(other._p_theme_park)
            , _desired(other._desired)
            , _sp_time_minutes() {

            if (other._sp_time_minutes != nullptr)
                _sp_time_minutes = std::make_unique<int>(*other._sp_time_minutes);
        }

        visit::~visit() {
        }

        int visit::time_in_park() {

            auto time_minutes_ = time_minutes();

            return time_minutes_ > _p_theme_park->max_minutes_per_day()
                ? _impossible
                : (time_minutes_ - _p_guest->_entry_time_minutes);
        }

        int visit::time_minutes() {

            if (_sp_time_minutes == nullptr)
                _sp_time_minutes = std::make_unique<int>(calculate_time());

            return *_sp_time_minutes;
        }

        int visit::calculate_time() {

            using challenges::trim_right;

            const int minutes_per_hour = constants::minutes_per_hour;
            const int wait_time = constants::wait_time;

            auto trimmed = trim_right(_desired, wait_time);

            auto current_time = _p_guest->_entry_time_minutes;

            for (auto i = 0; i < static_cast<int>(trimmed.size()); i++) {

                auto d = trimmed[i];

                if (d == wait_time) {
                    auto ceiling_time = (i + 1)*minutes_per_hour;
                    current_time = std::max(ceiling_time, current_time);
                    continue;
                }

                // When the decision is not to wait, then attend the attractions as frequently as time permits.
                auto & a = _p_theme_park->get_attraction(d);

                //var baseTime = entryTime > 0 ? (i*minutesPerHour + entryTime) : ((i + 1)*minutesPerHour);

                int queue_time;

                if (a.try_get_queue_time(current_time, queue_time))
                    current_time += queue_time;
            }

            return current_time;
        }

        bool visit::is_impossible() {
            return time_in_park() == _impossible;
        }
    }
}
